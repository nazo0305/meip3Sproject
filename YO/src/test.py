import numpy as np
import cv2
import basicFunctions as bf


# parameters
yellowMin = [16, 80, 70]
yellowMax = [36, 255, 255]


def main():
    cap = cv2.VideoCapture(0)
    while True:
        ret, frame = cap.read()
        try:
            cx, cy = bf.detect_red_color(frame)
            point = bf.calculateRectCornerByCenter(cx, cy)
            point = point.astype(np.int32)
            frame = bf.drawContour(point, frame)
        except TypeError:
            pass
        cv2.imshow("frame", frame)
        if cv2.waitKey(1) & 0xFF == ord('q'):
            break

    cap.release()
    cv2.destroyAllWindows()


if __name__ == "__main__":
    main()
