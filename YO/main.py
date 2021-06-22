import cv2
import basicFunctions as bf


def main():
    cap = cv2.VideoCapture(0)

    # # テンプレートマッチング用のテンプレート
    # template = cv2.imread("YO/img/ball3.png", 0)

    while True:
        sendItem = ""
        ret, frame = cap.read()
        gray = cv2.cvtColor(frame, cv2.COLOR_BGR2GRAY)

        # 円形検出と描画
        try:
            circles = bf.findCircles(gray)
            frame = bf.drawSmallCircles(frame, circles, maxR=100)
            # 座標出力
            sendItem = "c" + str(len(circles)) + "\n"
            for circle in circles:
                sendItem += "{} {} {}\n".format(circle[0][0],
                                                circle[0][1],
                                                circle[0][1])
        except (AttributeError, TypeError):
            pass

        # 矩形検出と描画
        frame, squares = bf.findSquares(gray, frame)
        squareNum = len(squares)
        if squareNum != 0:
            sendItem = sendItem + str(squareNum) + "\n"
            for square in squares:
                sendItem += "{} {} {} {} {} {} {} {}\n".format(
                    square[0], square[1], square[2], square[3], square[4],
                    square[5], square[6], square[7])

        # テンプレートマッチング
        # frame = bf.matchBallTemplate(gray, frame, template)

        # UDPでUnityに送信
        # print(sendItem)
        bf.sendInfoByUDP(sendItem)

        cv2.imshow("frame", frame)

        if cv2.waitKey(1) & 0xFF == ord('q'):
            break

    cap.release()
    cv2.destroyAllWindows()


if __name__ == "__main__":
    main()
