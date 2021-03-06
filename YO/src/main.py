import numpy as np
import cv2
import basicFunctions as bf


# 色範囲の指定(HSV)
blueMin = [90, 80, 80]
blueMax = [150, 255, 255]
greenMin = [50, 80, 80]
greenMax = [90, 255, 255]
yellowMin = [25, 80, 100]
yellowMax = [36, 255, 255]
tgtGreenMin = [32, 100, 100]
tgtGreenMax = [52, 255, 255]
tgtBlueMin = [90, 140, 160]
tgtBlueMax = [150, 255, 255]


def main(mode):
    ############# シューターモード ############
    if mode == "shooter":
        cap = cv2.VideoCapture(0)

        # frameごとの処理
        while True:
            sendItem = ""
            ret, frame = cap.read()
            hsv = cv2.cvtColor(frame, cv2.COLOR_BGR2HSV)
            gray = cv2.cvtColor(frame, cv2.COLOR_BGR2GRAY)

            # 円形検出と描画
            # try:
            #     circles = bf.findCircles(gray)
            #     frame = bf.drawSmallCircles(frame, circles, maxR=100)
            #     # 円データの書き出し
            #     # sendItem = bf.writeCirclesData(circles, sendItem)
            #     sendItem = sendItem + str(len(circles)) + "\n"
            #     for circle in circles:
            #         sendItem += "{} {} {}\n".format(circle[0][0], circle[0][1],
            #                                         circle[0][1])
            # except (AttributeError, TypeError):
            #     pass

            # 疑似円検出
            ballNum = 2
            # cv2.ellipse(frame, bf.getCenterAndRadius(hsv, redMin2, redMax2), (0, 255, 255))

            # # 赤色のボール
            # try:
            #     cx, cy, area = bf.detect_red_color(frame)
            #     point = bf.calculateRectCornerByCenter(cx, cy)
            #     r = np.sqrt(area/np.pi)
            #     point = point.astype(np.int32)
            #     frame = bf.drawContour(point, frame)
            #     sendItem += "{} {} {} {}\n".format("0", int(cx), int(cy), int(r))
            # except TypeError:
            #     ballNum -= 1

            # # 黄色のボール
            # try:
            #     center, r = bf.getCenterAndRadius(
            #         hsv, yellowMin, yellowMax)
            #     sendItem += "{} {} {} {}\n".format("0", center[0], center[1], r)
            # except TypeError:
            #     ballNum -= 1
            #     pass

            # 青色のボール
            try:
                center, r = bf.getCenterAndRadius(
                    hsv, blueMin, blueMax)
                sendItem += "{} {} {} {}\n".format("1", int(center[0]), int(center[1]), int(r))
                point = bf.calculateRectCornerByCenter(center[0], center[1])
                point = point.astype(np.int32)
                frame = bf.drawContour(point, frame)
            except TypeError:
                ballNum -= 1
                pass

            # 緑色のボール
            try:
                center, r = bf.getCenterAndRadius(
                    hsv, greenMin, greenMax)
                sendItem += "{} {} {} {}\n".format("2", int(center[0]), int(center[1]), int(r))
                point = bf.calculateRectCornerByCenter(center[0], center[1])
                point = point.astype(np.int32)
                frame = bf.drawContour(point, frame)
            except TypeError:
                ballNum -= 1
                pass

            sendItem = "0\n{}\n".format(ballNum) + sendItem

            # UDPでUnityに送信
            print(sendItem)
            bf.sendInfoByUDP(sendItem)

            cv2.imshow("frame", frame)

            if cv2.waitKey(1) & 0xFF == ord('q'):
                break
        cap.release()
        cv2.destroyAllWindows()

    ######### ターゲットモード ###########
    elif mode == "target":
        cap = cv2.VideoCapture(0)
        # qr = cv2.QRCodeDetector()
        while True:
            rectCount = 0
            sendItem = ""
            ret, frame = cap.read()
            hsv = cv2.cvtColor(frame, cv2.COLOR_BGR2HSV)
            # 画像を読み込めなければ終了
            if ret is False:
                break

            # # 白黒画像の作成
            # gray = cv2.cvtColor(frame, cv2.COLOR_BGR2GRAY)

            # # 矩形検出と描画
            # frame, squares = bf.findSquares(gray, frame)

            # # 矩形データの書き出し
            # sendItem = bf.writeSquaresData(squares, sendItem)

            # # QRコードの読み込み
            # try:
            #     (detected, decode_info,
            #      points, straight_qrcode) = qr.detectAndDecodeMulti(frame)
            #     if detected:
            #         itemNum = len(points)
            #         NullItemNum = decode_info.count('')
            #         sendItem += "{}\n".format(itemNum-NullItemNum)
            #         # QRコード認識位置を描画
            #         for i, point in enumerate(points):
            #             # 型の変換
            #             point = point.astype(np.int32)
            #             if decode_info[i] == "":
            #                 # print("undefined QR")
            #                 pass
            #             else:
            #                 sendItem = bf.writeCorners(
            #                     decode_info[i], point, sendItem)
            #                 # print(decode_info[i])
            #                 # print(point)
            #                 frame = bf.drawContour(point, frame)

            # except (AttributeError, TypeError):
            #     pass

            # get Rectangle Position by Color
            ## red
            try:
                cx, cy, _ = bf.detect_red_color(frame)
                point = bf.calculateRectCornerByCenter(cx, cy)
                point = point.astype(np.int32)
                frame = bf.drawContour(point, frame)
                sendItem = bf.writeCorners("0", point, sendItem)
                rectCount += 1
            except TypeError:
                pass

            # ## yellow
            # try:
            #     cx, cy = bf.getRectByColor(hsv, yellowMin, yellowMax)
            #     point = bf.calculateRectCornerByCenter(cx, cy)
            #     point = point.astype(np.int32)
            #     frame = bf.drawContour(point, frame)
            #     sendItem = bf.writeCorners("0", point, sendItem)
            #     rectCount += 1

            # except TypeError:
            #     pass

            ## blue
            try:
                cx, cy = bf.getRectByColor(hsv, tgtBlueMin, tgtBlueMax)
                point = bf.calculateRectCornerByCenter(cx, cy)
                point = point.astype(np.int32)
                frame = bf.drawContour(point, frame)
                sendItem = bf.writeCorners("1", point, sendItem)
                rectCount += 1

            except TypeError:
                pass

            ## green
            try:
                cx, cy = bf.getRectByColor(hsv, tgtGreenMin, tgtGreenMax)
                point = bf.calculateRectCornerByCenter(cx, cy)
                point = point.astype(np.int32)
                frame = bf.drawContour(point, frame)
                sendItem = bf.writeCorners("2", point, sendItem)
                rectCount += 1

            except TypeError:
                pass

            # UDPでUnityに送信
            # if sendItem == "" or sendItem == " ":
            #     sendItem += "0\n"

            sendItem = "{}\n".format(rectCount) + sendItem

            sendItem += "0\n"
            print(sendItem)
            bf.sendInfoByUDP(sendItem)

            cv2.imshow("frame", frame)

            if cv2.waitKey(1) & 0xFF == ord('q'):
                break

        cap.release()
        cv2.destroyAllWindows()

    else:
        raise ValueError("Unexpected mode.")


if __name__ == "__main__":
    modes = ["shooter", "target"]
    mode = modes[0]  # 0ならshooter, 1ならtarget
    main(mode)
