# The Python Tutorial chapter 7 Input and Output
# Note and tests from Peiren Yang 2016-07-29

# 7.1 Fancier Output Formatting

# s = 'Hello, world!'
# print(str(s))
# print(repr(s))
# print(str(1/7))
# str is used for human read, repr is used for interpreter recognise
# x = 10 * 3.25
# y = 200 ** 2
# s = 'The value of x is ' + repr(x) + ', and y is ' + repr(y) + '...'
# print(s)

# hello = 'hello, world\n'
# print(hello)
# print(repr(hello))

# for x in range(1, 11):
#     print(repr(x).rjust(2), repr(x ** 2).rjust(3), end=' ')
#     print(repr(x ** 3).rjust(4))

# for x in range(1, 11):
#     print('{0:2d} {1:3d} {2:4d}'.format(x, x ** 2, x ** 3))

# print('12'.zfill(5))
# print('-3.14'.zfill(7))
# print('3.14159265359'.zfill(5))

# print('We are the {} who say "{}!"'.format('knights', 'Ni'))
# print('{0} and {1}'.format('spam', 'eggs'))
# print('{1} and {0}'.format('spam', 'eggs'))
# print('{1} and {1}'.format('spam', 'eggs'))
# print('This {food} is {adjective}.'.format(food='spam', adjective='absolutely horrible'))
# print('The story of {0}, {1}, and {other}.'.format('Bill', 'Manfred', other='Georg'))

# from math import pi
# print('The value of PI is approximately {}.'.format(pi))
# print('The value of PI is approximately {!r}.'.format(pi))
# print('The value of PI is approximately {0:.3f}.'.format(pi))
# print('The value of PI is approximately %5.3f.' % math.pi)

# table = {'Sjoerd': 4127, 'Jack': 4098, 'Dcab': 7678}
# for name, phone in table.items():
#     print('{0:10} ==> {1:10d}'.format(name, phone))
# print('Jack: {0[Jack]:d}; Sjoerd: {0[Sjoerd]:d}; ''Dcab: {0[Dcab]:d}'.format(table))
# print('Jack: {Jack:d}; Sjoerd: {Sjoerd:d}; Dcab: {Dcab:d}'.format(**table))

# 7.2 Reading and Writing Files

f = open('work_file', 'w')

# 7.2.1 Methods of File Objects

# f.read(1000)
# f.readline()
# for line in f:
#     print(line, end='')
#
# print(list(f))
# f.write('This is a test\n')
# f.write(str(('the answer', 42)))

# f = open('work_file', 'rb+')
# print(f.write(b'0123456789abcdefghijkl'))
# print(f.seek(5))
# print(f.read(1))
# # from_what for f.seek, 0 beginning of the file, 1 current position, 2 end of the file
# print(f.seek(-3, 2))
# print(f.read(1))
# f.close()
#
# # better handle the files with 'with' keyword
# with open('work_file', 'r') as f:
#     read_data = f.readline()
# print(f.closed)

# 7.2.2 Saving structured data with json (objects' serializing and deserializing)
#
# import json
# json.dumps([1, 'simple', 'list'])
# json.loads("[1, 'simple', 'list']")

# pickle is specially used for python
