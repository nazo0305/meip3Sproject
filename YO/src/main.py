import numpy as np
import cv2
import basicFunctions as bf


def main(mode):
    ############# シューターモード ############
    if mode == "shooter":
        cap = cv2.VideoCapture(0)

        while True:
            sendItem = ""
            ret, frame = cap.read()
            gray = cv2.cvtColor(frame, cv2.COLOR_BGR2GRAY)

            # 円形検出と描画
            try:
                circles = bf.findCircles(gray)
                frame = bf.drawSmallCircles(frame, circles, maxR=100)
                # 円データの書き出し
                # sendItem = bf.writeCirclesData(circles, sendItem)
                sendItem = sendItem + str(len(circles)) + "\n"
                for circle in circles:
                    sendItem += "{} {} {}\n".format(circle[0][0], circle[0][1],
                                                    circle[0][1])
            except (AttributeError, TypeError):
                pass

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
                    sendItem += "{}\n".format(len(points))
                    # QRコード認識位置を描画
                    for i, point in enumerate(points):
                        # 型の変換
                        point = point.astype(np.int32)
                        sendItem = bf.writeCorners(decode_info[i], point, sendItem)
                        # print(decode_info[i])
                        # print(point)
                        frame = bf.drawContour(point, frame)

            except (AttributeError, TypeError):
                pass

            # UDPでUnityに送信
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
    mode = modes[1]  # 0ならshooter, 1ならtarget
    main(mode)
