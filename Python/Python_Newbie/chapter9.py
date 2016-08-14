# The Python Tutorial chapter 9 Classes
# Notes and Test Programs from Peiren Yang 2016-08-02

# derived class -- a class which inherited from another class

# 9.1 A Word About Names and Objects

# 9.2 Python Scopes and Namespaces


# def scope_test():
#     def do_local():
#         spam = 'local spam'
#
#     def do_nonlocal():
#         nonlocal spam
#         spam = 'nonlocal spam'
#
#     def do_global():
#         global spam
#         spam = 'global spam'
#     spam = 'test spam'
#     do_local()
#     print('After local assignment: ', spam)
#     do_nonlocal()
#     print('After nonlocal assignment:', spam)
#     do_global()
#     print('After global assignment:', spam)
#
# scope_test()
# print('In global scope:', spam)

# 9.3 A First Look at Classes

# 9.3.1 Class Definition Syntax

# 9.3.2 Class Objects

# Operations: Attribute references and instantiation

# class MyClass:
#     """My customized class"""
#     a = 123
#
#     def my_method(self):
#         return self
#
# x = MyClass()
# print(x.__doc__)

# class Complex:
#     def __init__(self, realpart, imagpart):
#         self.r = realpart
#         self.i = imagpart
#
#     def print_complex(self):
#         print('My value is {0} + {1}i'.format(self.r, self.i))
#
# x = Complex(3.0, - 4.5)
# x.print_complex()

# 9.3.3 Instance Objects

# 9.3.4 Method Objects

# 9.3.5 Class and Instance Variables

# class Dog:
#
#     def __init__(self, name):
#         self.name = name
#         self.tricks = [] # take care of here, problems may happen if self not been used
#
#     def add_trick(self, trick):
#         self.tricks.append(trick)
#
# d = Dog('Lily')
# e = Dog('Peiren')
#
# d.add_trick('Kick your ass!')
# e.add_trick('Kiss your ass!')
#
# print(d.name + ', I will ' + str(d.tricks))
# print(e.name + ', I will ' + str(e.tricks))

# 9.4 Random Remarks

# # crazy Python, such an unsafe grammar, maybe that is liberty
#
# def f1(self, x, y):
#     return min(x, y)
#
#
# class C:
#     f = f1
#
#     def g(self):
#         return 'hello world'
#     h = g
# c = C()
# print(c.f(1, 2))
# print(c.h())
# print(C.g(c))

# var = ' '.__class__
# print(var)

# 9.5 Inheritance

# isinstance(obj, SuperClassName) used for checking the inheritance relation
# issubclass(DrivedClassName, obj) used for checking the inheritance relation

# 9.5.1 Multiple Inheritance

# Allowed!!!

# class DerivedClassName(Base1, Base2, Base3):

# Here the monotonicity of the multi-inheritance
# will be kept by the dynamic algorithm

# 9.6 Private Variables

# two leading underscores are necessary for a private function

# 9.7 Odds and Ends

# class Employee:
#     pass
#
# john = Employee()
# john.name = 'John The'
# john.dept = 'computer lab'
# john.salary = 1000
#
# print(john)  # nothing will be printed

# 9.8 Exceptions Are Classes Too

# class B(Exception):
#     pass
#
#
# class C(B):
#     pass
#
#
# class D(C):
#     pass
#
# for cls in [B, C, D]:
#     try:
#         raise cls()
#     except D:
#         print("D")
#     except C:
#         print("C")
#     except B:
#         print("B")
#
# for cls in [B, C, D]:
#     try:
#         raise cls()
#     except B:
#         print("B")
#     except C:
#         print("C")
#     except D:
#         print("D")
#

# 9.9 Iterators
#
# for element in [1, 2, 3]:
#     print(element)
#
# for element in (1, 2, 3):
#     print(element)
#
# for key in {'one':1, 'two':2}:
#     print(key)
#
# for char in '123':
#     print(char)

# s = 'abc'
# it = iter(s)
# print(next(it))
# print(next(it))
# print(next(it))


# Here the for sentence accept the StopIteration exception and stop looping

# class Reverse:
#     """Iterator for looping over a sequence backwards."""
#
#     def __init__(self, data):
#         self.data = data
#         self.index = len(data)
#
#     def __iter__(self):
#         return self
#
#     def __next__(self):
#         if self.index == 0:
#             raise StopIteration
#         self.index -= 1
#         return self.data[self.index]
#
# rev = Reverse('hello')
#
# for char in rev:
#     print(char)

# 9.10 Generators

# def reverse(data):
#     for index in range(len(data)-1, -1, -1):
#         yield data[index]
#
# for char in reverse('hello'):
#     print(char)

# 9.11 Generator Expressions

# print(sum(i*i for i in range(10)))
#
# data = 'hello'
# print(list(data[i] for i in range(len(data)-1, -1, -1)))
