import cv2
import basicFunctions as bf


def main():
    cap = cv2.VideoCapture(0)

    # テンプレートマッチング用のテンプレート
    template = cv2.imread("YO/img/ball3.png", 0)

    while(True):
        ret, frame = cap.read()
        gray = cv2.cvtColor(frame, cv2.COLOR_BGR2GRAY)

        # 円形検出と描画
        try:
            circles = bf.findCircles(gray)
            frame = bf.drawSmallCircles(frame, circles, maxR=100)
        except (AttributeError, TypeError):
            pass

        # 矩形検出と描画
        frame = bf.findSquares(gray, frame)

        # テンプレートマッチング
        frame = bf.matchBallTemplate(gray, frame, template)
        cv2.imshow("frame", frame)

        if cv2.waitKey(1) & 0xFF == ord('q'):
            break

    cap.release()
    cv2.destroyAllWindows()


if __name__ == "__main__":
    main()
