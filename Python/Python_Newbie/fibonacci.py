# Fibonacci number generation function
# Copied from Python documentation with its authorisation
# 2016-07-29 Peiren Yang


def fib_print(n):
    """print Fibonacci series"""
    a, b = 0, 1
    while b < n:
        print(b, end=' ')
        a, b = b, a + b
    print()


def fib_create(n):
    """create Fibonacci list"""
    result = []
    a, b = 0, 1
    while b < n:
        result.append(b)
        a, b = b, a + b
    return result


# call the module directly like $ python fibonacci.py 50
if __name__ == "__main__":
    import sys
    fib_print(int(sys.argv[1]))