import sys
import cv2

sys.path.append('../src/')

import extract


def main():
    cap = cv2.VideoCapture(0)

    while 1:
        ret, frame = cap.read()
        cv2.imshow("frame", frame)

        cv2.imshow("extracted", extract.range(frame, [127, 127, 127], [255, 255, 255]))

        if cv2.waitKey(30) & 0xFF == ord('q'):
            break

    cap.release()
    cv2.destroyAllWindows()


if __name__ == '__main__':
    main()
