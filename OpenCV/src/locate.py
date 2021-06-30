import cv2
import numpy


def circle(image, lower, upper):
    """
    @param `image` BGR / HSV color space.
    @param `lower` Lowerbounds for BGR / HSV.
    @param `upper` Upperbounds for BGR / HSV.
    @return (Center, Radius): tuple. All values are of type float. Return `None` if failed.
    """

    binary = cv2.inRange(image, numpy.array(lower), numpy.array(upper))
    kernel = cv2.getStructuringElement(cv2.MORPH_ELLIPSE, (10, 10))
    morph = cv2.morphologyEx(binary, cv2.MORPH_OPEN, kernel)
    contours, hierarchy = cv2.findContours(morph, cv2.RETR_LIST, cv2.CHAIN_APPROX_SIMPLE)
    # cv2.drawContours(image, contours, -1, (0, 0, 255), 3)

    if not contours:
        return None

    cont = max(contours, key=cv2.contourArea)
    if len(cont) < 5:
        return None

    (x, y), (h, w), theta = cv2.fitEllipse(cont)
    return ((x, y), min(h, w))
