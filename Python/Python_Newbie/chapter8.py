# Python Tutorial chapter 8 Errors and Exceptions
# Notes and Tests from Peiren Yang 2016-07-31


# 8.1 Syntax Errors
# Newbie error

# 8.2 Exceptions

# 8.3 Handling Exceptions

# while True:
#     try:
#         int(input('Please input an integer number: '))
#         break
#     except ValueError:
#         print('The input number is not an Integer, please try again...')

# try:
#     raise Exception('Test Exception', 'Supplementary information')
# except Exception as inst:
#     print(type(inst))
#     print(inst.args)
#     print(inst)
#     print()
#     print(inst.args[0])
#     print(inst.args[-1])

# 8.4 Raising Exceptions

# try:
#     raise NameError('Test')
# except NameError:
#     raise

# first Python class, the parameter in the bracket is the superclass name
# class MyError(Exception):
#     def __init__(self, value):
#         self.value = value
#
#     def __str__(self):
#         return repr(self.value)
# try:
#     raise MyError(4)
# except MyError as e:
#     print('Customized error occurred, value: ', e.value)


# class Error(Exception):
#     """Nothing but an inheritance of Exception class"""
#     pass
#
#
# class InputError(Error):
#     """Exception raised for errors in an expression
#
#     Attributes:
#         expression -- input expression, where error occurs
#         message -- error message
#     """
#
#     def __init__(self, expression, message):
#         self.expression = expression
#         self.message = message

# 8.6 Defining Clean-up Actions

# def divide(x, y):
#     try:
#         result = x / y
#         print('result is', result)
#     except ZeroDivisionError:
#         print('division by zero!')
#     # else:
#     #     print('result is', result)
#     finally:
#         print('Thank you for using this function!')
#
# divide(2, 1)
# divide(2, 0)

# 8.7 Predefined Clean-up Actions

# A safe way to use objects like files
# with open("myfile.txt") as f:
#     for line in f:
#         print(line, end="")
