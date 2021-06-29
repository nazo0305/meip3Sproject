import numpy as np
import cv2


def main():
    cap = cv2.VideoCapture(0)
    qr = cv2.QRCodeDetector()

    while True:
        ret, frame = cap.read()
        try:
            (detected, decode_info,
             points, straight_qrcode) = qr.detectAndDecodeMulti(frame)
            if detected:
                print('QRデータ:{}'.format(decode_info))

            # QRコード認識位置を描画
            for point in points:
                # 型の返還
                point = point.astype(np.int32)
                print(point)
                frame = cv2.polylines(frame, [point], True, (0, 0, 255), 2, cv2.LINE_AA)

        except (AttributeError, TypeError):
            pass

        cv2.imshow("frame", frame)

        if cv2.waitKey(1) & 0xFF == ord('q'):
            break


if __name__ == "__main__":
    main()
