# From Peiren Yang
# 2016.11.02
# Matrix multiplication for matrix translation and rotation
# Program for industry robot at University Augsburg, Exercise 2

import numpy
import math


def transformation_3d(t_vector):
    return numpy.matrix([[1, 0, 0, t_vector[0]], [0, 1, 0, t_vector[1]], [0, 0, 1, t_vector[2]], [0, 0, 0, 1]])


def rotation_3d(axis, angle_d):
    angle = (angle_d/180) * math.pi
    return Axis.get(axis)(angle)


def rotation_3dx(angle):
    return numpy.matrix([[1, 0, 0, 0], [0, math.cos(angle), -math.sin(angle), 0], [0,  math.sin(angle), math.cos(angle), 0],
                     [0, 0, 0, 1]])


def rotation_3dy(angle):
    return numpy.matrix([[math.cos(angle), 0, math.sin(angle), 0], [0, 1, 0, 0], [-math.sin(angle),  0, math.cos(angle), 0],
                     [0, 0, 0, 1]])


def rotation_3dz(angle):
    return numpy.matrix([[math.cos(angle), -math.sin(angle), 0, 0], [math.sin(angle), math.cos(angle), 0, 0], [0,  0, 1, 0],
                     [0, 0, 0, 1]])

Axis = {'X': rotation_3dx, 'Y': rotation_3dy, 'Z': rotation_3dz}


def main():
    o_point_a = [1, 0, 0]
    o_point_b = [1, 2, 0]
    o_point_c = [2, 1, 2]
    o_point_d = [0, 0, 2]
    # Homogeneous vector
    point_a = [1, 0, 0, 1]
    point_b = [1, 2, 0, 1]
    point_c = [2, 1, 2, 1]
    point_d = [0, 0, 2, 1]
    trans1 = numpy.dot(numpy.dot(numpy.dot(rotation_3d('X', 30), transformation_3d([1, 2, 3])), rotation_3d('Y', -45)),
                       transformation_3d([0, 2, 0]))
    trans2 = numpy.dot(numpy.dot(numpy.dot(rotation_3d('X', 30), transformation_3d([1, 2, 3])), transformation_3d([0, 2, 0])),
                       rotation_3d('Y', -45))
    print('Answer 1 is:')
    print('A = ', (numpy.dot(trans1, point_a)).A[0][0:3])
    print('B = ', (numpy.dot(trans1, point_b)).A[0][0:3])
    print('C = ', (numpy.dot(trans1, point_c)).A[0][0:3])
    print('D = ', (numpy.dot(trans1, point_d)).A[0][0:3])
    print()
    print('Answer 2 is:')
    print('A = ', (numpy.dot(trans2, point_a)).A[0][0:3])
    print('B = ', (numpy.dot(trans2, point_b)).A[0][0:3])
    print('C = ', (numpy.dot(trans2, point_c)).A[0][0:3])
    print('D = ', (numpy.dot(trans2, point_d)).A[0][0:3])

if __name__ == "__main__":
    main()