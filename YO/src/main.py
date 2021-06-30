import numpy as np
import cv2
import basicFunctions as bf


def main(mode):
    ############# シューターモード ############
    if mode == "shooter":
        cap = cv2.VideoCapture(0)
        # 色範囲の指定(HSV)
        redMin1 = [0, 64, 0]
        redMax1 = [30, 255, 255]
        redMin2 = [150, 200, 200]
        redMax2 = [179, 255, 255]
        blueMin = [90, 180, 180]
        blueMax = [150, 255, 255]
        greenMin = [30, 150, 150]
        greenMax = [90, 255, 255]

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
            ballNum = 3
            # cv2.ellipse(frame, bf.getCenterAndRadius(hsv, redMin2, redMax2), (0, 255, 255))

            # 赤色のボール
            try:
                center, r = bf.getCenterAndRadius(
                    hsv, redMin2, redMax2)
                sendItem += "{} {} {} {}\n".format("R", center[0], center[1], r)
            except TypeError:
                ballNum -= 1
                pass

            # 青色のボール
            try:
                center, r = bf.getCenterAndRadius(
                    hsv, blueMin, blueMax)
                sendItem += "{} {} {} {}\n".format("B", center[0], center[1], r)
            except TypeError:
                ballNum -= 1
                pass

            # 緑色のボール
            try:
                center, r = bf.getCenterAndRadius(
                    hsv, greenMin, greenMax)
                sendItem += "{} {} {} {}\n".format("G", center[0], center[1], r)
            except TypeError:
                ballNum -= 1
                pass

            sendItem = "{}\n".format(ballNum) + sendItem

            # UDPでUnityに送信
            print(sendItem)
            # bf.sendInfoByUDP(sendItem)

            cv2.imshow("frame", frame)

            if cv2.waitKey(1) & 0xFF == ord('q'):
                break
        cap.release()
        cv2.destroyAllWindows()

    ######### ターゲットモード ###########
    elif mode == "target":
        cap = cv2.VideoCapture(0)
        qr = cv2.QRCodeDetector()
        while True:
            sendItem = ""
            ret, frame = cap.read()
            # 画像を読み込めなければ終了
            if ret is False:
                break

            # 白黒画像の作成
            gray = cv2.cvtColor(frame, cv2.COLOR_BGR2GRAY)

            # # 矩形検出と描画
            # frame, squares = bf.findSquares(gray, frame)

            # # 矩形データの書き出し
            # sendItem = bf.writeSquaresData(squares, sendItem)

            # QRコードの読み込み
            try:
                (detected, decode_info,
                 points, straight_qrcode) = qr.detectAndDecodeMulti(frame)
                if detected:
                    itemNum = len(points)
                    NullItemNum = decode_info.count('')
                    sendItem += "{}\n".format(itemNum-NullItemNum)
                    # QRコード認識位置を描画
                    for i, point in enumerate(points):
                        # 型の変換
                        point = point.astype(np.int32)
                        if decode_info[i] == "":
                            # print("undefined QR")
                            pass
                        else:
                            sendItem = bf.writeCorners(
                                decode_info[i], point, sendItem)
                            # print(decode_info[i])
                            # print(point)
                            frame = bf.drawContour(point, frame)

            except (AttributeError, TypeError):
                pass

            # UDPでUnityに送信
            if sendItem == "" or sendItem == " ":
                sendItem = "0\n"
            print(sendItem)
            # bf.sendInfoByUDP(sendItem)

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
