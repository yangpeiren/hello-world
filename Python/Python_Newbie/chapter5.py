# 2016-07-28 for learning the python handbook chapter 5 data structures, Peiren Yang

# 5.1.1 List as Stacks
# stack = [1, 2, 3]
# stack.append(4)
# print(stack)
# stack.append(5)
# print(stack)
# stack.pop()
# print(stack)
# stack.pop()
# print(stack)

# 5.1.2 List as Queues
# from collections import deque
# queue = deque(["Zhangsan", "Lisi", "Wangmazi"])
# queue.append("Zhaowu")
# print(queue)
# queue.append("Maliu")
# print(queue)
# print(queue.popleft())
# print(queue.popleft())
# print(queue)

# 5.1.3 List Comprehensions
# squares = []
# for i in range(10):
#     squares.append(i ** 2)
# print(squares)

# The two ways below really amazed me, something new for python
# squares = list(map(lambda i: i ** 2, range(10)))
# print(squares)

# squares = [i ** 2 for i in range(10)]
# print(squares)

# print([(x, y) for x in [3, 1, 4] for y in [2, 4, 6] if x != y])

# vec = [-4, -2, 0, 2, 4]
# print([x for x in vec if x > 0])
# print([x ** 2 for x in vec])
# print([abs(x) for x in vec])

# frucht = ['  banane', 'loganbeere ', 'passionfrucht   ']
# print([weapon.strip() for weapon in frucht])
#
# print([(x, x * 2) for x in range(6)])
#
# vector flatten
# vec = [(1, 2, 3), (4, 5, 6), (7, 8, 9)]
# print([num for elem in vec for num in elem])

# 5.1.4 Nested List Comprehensions

# matrix = [[1, 2, 3, 4], [5, 6, 7, 8], [9, 10, 11, 12]]
# print([[row[i] for row in matrix] for i in range(4)])
#
# print(list(zip(*matrix)))

# 5.2 The del statement
#
# a = [1, 3, 5, 7, 9]
# del a[1]
# print(a)
# del a[2:]
# print(a)

# 5.3 Tuples and Sequences
#
# t = 12345, 54321, 'world_peace'
# print(t[0])
# u = t, (1, 2, 3, 4, 5)
# print(u)

# v = ([1, 2, 3], [3, 2, 1])
# v[0][0] = 10
# print(v)

# x, y, z = t

# 5.4 Sets
# empty set
# a = set()

# basket = {'apple', 'pear', 'orange', 'apple'}
# print(basket)
# print('orange' in basket)
# print('banana' in basket)

# a = set('abcdabcaba')
# b = set('cdefgac')
# print(a - b)
# print(a | b)
# print(a & b)
# Below is a xor relationship
# print(a ^ b)

# Same initialization rule of list
# print({x for x in 'abcdabcef' if x not in 'abc'})

# 5.5 Dictionaries
#
# tel = {'jack': 12345, 'rose': 23456}
# tel['peiren'] = 666666
# print(tel)
# del tel['rose']
# tel['ivl'] = 23456
# print(tel)
# list(tel.keys())
# sorted(tel.keys())
# 'peiren' in tel
# 'jack' not in tel

# dict([('sape', 4138), ('peiren', 6666), ('morrow', 8888)])

# {x: x**2 for x in (2, 4, 6)}

# dict(sape=4138, peiren=6666, morrow=8888)

# 5.6 Looping Techniques
#
#  knights = {'gallahad': 'the pure', 'robin': 'the brave'}
# for k, v in knights.items():
#     print(k, v)

# questions = ['name', 'quest', 'favorite color']
# answers = ['lancelot', 'the holy grail', 'blue']
# for q, a in zip(questions, answers):
#     print('What is you {0}? It\'s {1}.'.format(q, a))

# basket = ['apple', 'orange', 'apple', 'pear', 'orange', 'banana']
# for f in reversed(sorted(set(basket))):
#     print(f)

# import math
# raw_data = [56.2, float('NaN'), 51,7, 55.3, 52.5, float('NaN'), 47.8]
# temp_data = [x for x in raw_data if not math.isnan(x)]
# print(temp_data)

# 5.7 & 5.8 Conditions and Sequence compare & Other Types
# Condition rules are the same as Java or C#
# The comparison uses lexicographical ordering:
# first the first two items are compared,
# and if they differ this determines the outcome of the comparison;
# if they are equal, the next two items are compared,
# and so on, until either sequence is exhausted

# print((1, 2, 3)< (1, 2, 4))
# print([1, 2, 3]< [1, 2, 4])
# print('ABC' < 'C' < 'Pascal' < 'Python')
# print((1, 2, 3, 4)< (1, 2, 4))
# print((1, 2)< (1, 2, -1))
# print((1, 2, 3)== (1.0, 2.0, 3.0))
# print((1, 2, ('aa', 'ab'))< (1, 2, ('abc', 'a'), 4))
