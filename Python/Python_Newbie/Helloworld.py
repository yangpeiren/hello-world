#! /usr/bin/env python
""" hello_world.py, first python program by Peiren Yang, 2016-07-25
This is a test program for me to get start of python
"""
import random


def main():

    print('Guess a number between 1 and 100')
    # for debug
    # random_number = 45
    random_number = random.randint(0,100)
    found = False

    while not found:
        user_guess = int(input('Your guess: '))
        if user_guess == random_number:
            print("You got it!")
            found = True
        elif random_number > user_guess:
            print('Too small!')
        else:
            print('Too big!')
    print("Thank you for play, goodbye!")

if __name__ == "__main__":
    main()
