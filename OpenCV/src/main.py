import cv2
import utils

# 背景オブジェクト
# .apply(image)で差分取得
subtractor = cv2.createBackgroundSubtractorMOG2()


def main():
    capture = cv2.VideoCapture(0)


if __name__ == "__main__":
    main()
