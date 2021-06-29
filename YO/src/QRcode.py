import numpy as np
import basicFunctions as bf
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
                    # 型の変換
                    point = point.astype(np.int32)
                    print(point)
                    frame = bf.drawContour(point, frame)

        except (AttributeError, TypeError):
            pass

        cv2.imshow("frame", frame)

        if cv2.waitKey(1) & 0xFF == ord('q'):
            break


if __name__ == "__main__":
    main()
