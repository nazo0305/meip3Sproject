import sys
import cv2

sys.path.append('../src/')

import locate


def main():
    cap = cv2.VideoCapture(0)

    while 1:
        ret, frame = cap.read()
        cv2.imshow("frame", frame)

        ret = locate.circle(frame, [200, 200, 200], [255, 255, 255])
        located = frame
        if ret:
            (x, y), r = ret
            located = cv2.circle(frame, (int(x), int(y)), int(r), (0, 255, 0))
        cv2.imshow("located", located)

        if cv2.waitKey(30) & 0xFF == ord('q'):
            break

    cap.release()
    cv2.destroyAllWindows()


if __name__ == '__main__':
    main()
