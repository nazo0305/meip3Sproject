import cv2
import numpy


def range(image, lower, upper):
    """
    @param image BGR / HSV color space.
    @param lower Lowerbounds for BGR / HSV.
    @param upper Upperbounds for BGR / HSV.
    """
    return cv2.bitwise_and(image, image, mask=cv2.inRange(image, numpy.array(lower), numpy.array(upper)))
