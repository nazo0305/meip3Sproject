import cv2
import socket
import numpy as np
import math


def findCircles(grayImg):
    '''
    グレースケール画像を受け取り，円のリストを返す．
    HoughCircles()内部でCannny法も実行している．
    '''
    circles = cv2.HoughCircles(
        image=grayImg,
        method=cv2.HOUGH_GRADIENT,
        dp=1,
        minDist=50,
        param1=100,
        param2=80)
    circles = np.uint(np.around(circles))
    return circles


def drawSmallCircles(img, circles, maxR):
    '''
    画像と円のリストを受け取り，与えられたmaxR以下の半径を持つ円を画像上に描画する．
    '''
    for circle in circles[0, :]:
        # 円が検出されなかった場合[0 0 0]が渡されるのでそれは省きたい
        if 0 < circle[2] <= maxR:
            # 円周を描く
            cv2.circle(img, (circle[0], circle[1]), circle[2], (0, 255, 0), 2)
            # 円の中心を描く
            cv2.circle(img, (circle[0], circle[1]), 2, (0, 0, 255), 3)
    return img


def angle(pt1, pt2, pt0) -> float:
    '''
    pt0-> pt1およびpt0-> pt2からの
    ベクトル間の角度の余弦(コサイン)を算出
    '''
    dx1 = float(pt1[0, 0] - pt0[0, 0])
    dy1 = float(pt1[0, 1] - pt0[0, 1])
    dx2 = float(pt2[0, 0] - pt0[0, 0])
    dy2 = float(pt2[0, 1] - pt0[0, 1])
    v = math.sqrt((dx1*dx1 + dy1*dy1)*(dx2*dx2 + dy2*dy2))
    return (dx1*dx2 + dy1*dy2) / v


def findSquares(grayImg, img, cond_area=1000):
    '''
    参考:https://qiita.com/sitar-harmonics/items/ac584f99043574670cf3
    長方形を検出し，画像上に青の長方形を描画する．
    '''
    # 四角形の頂点を戻り値として返す
    retSquares = []
    # 2値画像の作成
    _, binImg = cv2.threshold(grayImg, 0, 255, cv2.THRESH_OTSU)
    # 輪郭取得con
    contours, dummy = cv2.findContours(binImg,
                                       cv2.RETR_LIST,
                                       cv2.CHAIN_APPROX_SIMPLE)
    for cnt in contours:
        # 輪郭の周囲に比例する精度で輪郭を近似する
        arclen = cv2.arcLength(cnt, True)
        approx = cv2.approxPolyDP(cnt, arclen*0.02, True)

        # 四角形の輪郭は、近似後に4つの頂点がある
        # 比較的広い領域が凸状になる

        # 凸性の確認
        area = abs(cv2.contourArea(approx))
        if (approx.shape[0] == 4 and area > cond_area
                and cv2.isContourConvex(approx)):
            maxCosine = 0

            for j in range(2, 5):
                # 辺間の角度の最大コサインを算出
                cosine = abs(angle(approx[j % 4], approx[j-2], approx[j-1]))
                maxCosine = max(maxCosine, cosine)

            # すべての角度の余弦定理が小さい場合
            # （すべての角度は約90度）次に、quandrangeを書き込む
            # 結果のシーケンスへの頂点
            if maxCosine < 0.3:
                # 四角判定
                rcnt = approx.reshape(-1, 2)  # 四隅の点
                cv2.polylines(img, [rcnt], True, (255, 0, 0),
                              thickness=2, lineType=cv2.LINE_8)
                retSquares.append(list(rcnt.ravel()))
    return img, retSquares


def matchBallTemplate(grayImg, img, template):
    '''
    templateの画像サイズと同じサイズで映像に映ることを想定したプログラムのため，
    今回のようなボールの大きさが変わりうる場合の検出には不適かもしれない．
    '''
    w, h = template.shape[::-1]
    res = cv2.matchTemplate(grayImg, template, cv2.TM_CCOEFF_NORMED)
    threshold = 0.75
    loc = np.where(res >= threshold)
    for pt in zip(*loc[::-1]):
        cv2.rectangle(img, pt, (pt[0]+w, pt[1]+h), (0, 0, 255), 2)
    return img


def writeSquaresData(squares, sendStr):
    '''
    認識した矩形のデータをUDP通信で送るフォーマットにして記入する

    Parameters
    ----------
    squares: Numpy.array
    sendStr: str
        UDP通信で送る文字列データ
    '''
    squareNum = len(squares)
    if squareNum != 0:
        sendStr = sendStr + str(squareNum) + "\n"
        for square in squares:
            sendStr += "{} {} {} {} {} {} {} {}\n".format(
                square[0], square[1], square[2], square[3], square[4],
                square[5], square[6], square[7])
    print(sendStr)
    return sendStr


def writeCirclesDate(circles, sendStr):
    sendStr = sendStr + str(len(circles)) + "\n"
    for circle in circles:
        sendStr += "{} {} {}\n".format(circle[0][0], circle[0][1],
                                       circle[0][1])
    print(sendStr)
    return sendStr


def sendInfoByUDP(item):
    '''
    （仮）
    itemをUDP通信で送信する
    参考:https://jump1268.hatenablog.com/entry/2018/11/25/143459
    '''

    HOST = '127.0.0.1'
    PORT = 50007

    client = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
    sendItem = str(item)
    if sendItem == "":
        sendItem = 0
    client.sendto(sendItem.encode('utf-8'), (HOST, PORT))


def drawContour(point, frame):
    frame = cv2.polylines(frame, [point], True, (0, 0, 255), 2, cv2.LINE_AA)
    return frame


def writeCorners(title, point, sendStr):
    sendStr += "{}".format(title)
    for i in range(len(point)):
        sendStr += " {} {}".format(point[i][0], point[i][1])
    sendStr += "\n"
    return sendStr


# without morphology
# def getCenterAndRadius(image, lower, upper):
#     """
#     @param `image` BGR / HSV color space.
#     @param `lower` Lowerbounds for BGR / HSV.
#     @param `upper` Upperbounds for BGR / HSV.
#     @return (Center, Radius): tuple. All values are of type float. Return `None` if failed.
#     """

#     binary = cv2.inRange(image, np.array(lower), np.array(upper))
#     contours, hierarchy = cv2.findContours(binary, cv2.RETR_LIST, cv2.CHAIN_APPROX_SIMPLE)
#     # image = cv2.drawContours(image, contours, -1, (0, 0, 255), 3)

#     if not contours:
#         return None

#     cont = max(contours, key=cv2.contourArea)
#     if len(cont) < 5:
#         return None

#     # return cv2.fitEllipse(cont)
#     (x, y), (h, w), theta = cv2.fitEllipse(cont)
#     return ((x, y), min(h, w))

# with morphology
def getCenterAndRadius(image, lower, upper):
    """
    @param `image` BGR / HSV color space.
    @param `lower` Lowerbounds for BGR / HSV.
    @param `upper` Upperbounds for BGR / HSV.
    @return (Center, Radius): tuple. All values are of type float. Return `None` if failed.
    """

    binary = cv2.inRange(image, np.array(lower), np.array(upper))
    kernel = cv2.getStructuringElement(cv2.MORPH_ELLIPSE, (10, 10))
    morph = cv2.morphologyEx(binary, cv2.MORPH_OPEN, kernel)
    contours, hierarchy = cv2.findContours(
        morph, cv2.RETR_LIST, cv2.CHAIN_APPROX_SIMPLE)
    # cv2.drawContours(image, contours, -1, (0, 0, 255), 3)

    if not contours:
        return None

    cont = max(contours, key=cv2.contourArea)
    if len(cont) < 5:
        return None

    (x, y), (h, w), theta = cv2.fitEllipse(cont)
    return ((x, y), min(h, w))


def getRectByColor(image, lower, upper):
    binary = cv2.inRange(image, np.array(lower), np.array(upper))
    kernel = cv2.getStructuringElement(cv2.MORPH_ELLIPSE, (10, 10))
    morph = cv2.morphologyEx(binary, cv2.MORPH_OPEN, kernel)
    contours, _ = cv2.findContours(
        morph, cv2.RETR_LIST, cv2.CHAIN_APPROX_SIMPLE)

    if not contours:
        return None

    cont = max(contours, key=cv2.contourArea)
    if len(cont) < 5:
        return None

    M = cv2.moments(cont)
    cx = int(M['m10']/M['m00'])
    cy = int(M['m01']/M['m00'])
    return cx, cy


def calculateRectCornerByCenter(cx, cy):
    rectWidth = 100
    rectHeight = 100
    return np.array([[cx-rectWidth//2, cy-rectWidth//2],
                     [cx-rectWidth//2, cy+rectWidth//2],
                     [cx+rectWidth//2, cy+rectWidth//2],
                     [cx+rectWidth//2, cy-rectWidth//2]])


def detect_red_color(img):
    kernel = np.ones((11,11),np.uint8)

    # HSV色空間に変換
    hsv = cv2.cvtColor(img, cv2.COLOR_BGR2HSV)

    # 赤色のHSVの値域1
    hsv_min = np.array([0,10,10])
    hsv_max = np.array([30,255,255])
    mask1 = cv2.inRange(hsv, hsv_min, hsv_max)

    # 赤色のHSVの値域2
    hsv_min = np.array([150,10,30])
    hsv_max = np.array([179,255,255])
    mask2 = cv2.inRange(hsv, hsv_min, hsv_max)

    # 赤色領域のマスク（255：赤色、0：赤色以外）    
    mask = mask1 + mask2

    # morphology
    mask = cv2.morphologyEx(mask, cv2.MORPH_OPEN, kernel)

    # # マスキング処理
    # masked_img = cv2.bitwise_and(img, img, mask=mask)

    contours, _ = cv2.findContours(
        mask, cv2.RETR_LIST, cv2.CHAIN_APPROX_SIMPLE)

    if not contours:
        return None

    cont = max(contours, key=cv2.contourArea)
    if len(cont) < 5:
        return None

    area = cv2.contourArea(cont)
    if area < 4000:
        return None

    M = cv2.moments(cont)
    cx = int(M['m10']/M['m00'])
    cy = int(M['m01']/M['m00'])
    return cx, cy, area
