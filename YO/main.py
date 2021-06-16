import cv2
import basicFunctions as bf


def main():
    cap = cv2.VideoCapture(0)
    while(True):
        ret, frame = cap.read()
        gray = cv2.cvtColor(frame, cv2.COLOR_BGR2GRAY)
        try:
            circles = bf.findCircles(gray)
            frame = bf.drawSmallCircles(frame, circles, maxR=100)
        except (AttributeError, TypeError):
            pass

        cv2.imshow("frame", frame)

        if cv2.waitKey(1) & 0xFF == ord('q'):
            break

    cap.release()
    cv2.destroyAllWindows()


if __name__ == "__main__":
    main()
