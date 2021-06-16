import cv2
import socket
import numpy as np
import matplotlib.pyplot as plt


def findCircles(grayImg):
    '''
    グレースケール画像を受け取り，円のリストを返す．
    HoughCircles()内部でCannny法も実行している．
    '''
    circles = cv2.HoughCircles(
                image=grayImg,
                method=cv2.HOUGH_GRADIENT,
                dp=1,
                minDist=50,
                param1=100,
                param2=80)
    circles = np.uint(np.around(circles))
    return circles


def drawSmallCircles(img, circles, maxR):
    '''
    画像と円のリストを受け取り，与えられたmaxR以下の半径を持つ円を画像上に描画する．
    '''
    for circle in circles[0, :]:
        # 円が検出されなかった場合[0 0 0]が渡されるのでそれは省きたい
        if 0 < circle[2] <= maxR:
            # 円周を描く
            cv2.circle(img, (circle[0], circle[1]), circle[2], (0, 255, 0), 2)
            # 円の中心を描く
            cv2.circle(img, (circle[0], circle[1]), 2, (0, 0, 255), 3)
    return img


def sendInfoByUDP(item):
    '''
    （仮）
    itemをUDP通信で送信する
    参考:https://jump1268.hatenablog.com/entry/2018/11/25/143459
    '''

    HOST = '127.0.0.1'
    PORT = '50007'

    client = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
    sendItem = str(item)
    client.sendto(sendItem.encode('utf-8'), (HOST, PORT))
