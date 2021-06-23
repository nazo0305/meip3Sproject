# 背景差分のテスト

import cv2


def main():
    cap = cv2.VideoCapture(0)
    subtractor = cv2.createBackgroundSubtractorMOG2(detectShadows=False)
    kernel = cv2.getStructuringElement(cv2.MORPH_ELLIPSE, (3, 3))

    while 1:
        ret, frame = cap.read()
        cv2.imshow("frame", frame)

        diff = subtractor.apply(frame)
        cv2.imshow('diff', diff)

        # モーフォロジカル処理
        diff_morp = cv2.morphologyEx(diff, cv2.MORPH_OPEN, kernel)
        cv2.imshow('diff_morp', diff_morp)

        if cv2.waitKey(30) & 0xFF == ord('q'):
            break

    cap.release()
    cv2.destroyAllWindows()


if __name__ == "__main__":
    main()
