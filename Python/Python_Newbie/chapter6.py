# 2016-07-29 tests Python documentation chapter 6 Modules
# From Peiren Yang

# 6.1. More on Modules
# import fibonacci
#
# fibonacci.fib_print(1000)
# print(fibonacci.fib_create(100))
# print(fibonacci.__name__)

# modname.itemname this way of fuction calling is the same of C#
# between local and global fuctions, no worry about clashes

# direct name import

# from fibonacci import fib_print, fib_create
# fib_print(500)
# print(fib_create(200))

# from fibonacci import *
# fib_print(300)

# After changing the imported module, use imp.reload() to refresh it

# 6.1.1 Executing modules as scripts

# 6.1.2 The Module Search Path

# sys.path -> script directory, PYTHONPATH, installation-dependent default

# 6.1.3 "Compiled" Python files

# pycache cache the pyc file, compiled py files, directory __pycache__
# -o removes assert statements, -oo removes furthermore __doc__ strings.
# compileall creates .pyc files for all modules in a directory

# 6.2 Standard Modules

# sys.ps1 and sys.ps2, they seem not exist? not important

# sys.path.append('/...') enlarge the sys path

# 6.3 The dir() Function

# dir() current defined variables

# dir(sys)

# 6.4 Packages

# __init__.py  are required to make Python treat the directories as containing packages
# __all__ should be declared in the __init__.py, to declare the functions

# relative import works, better not use



