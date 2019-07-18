import random

print("Let's play a guessing game.")
print("You're going to pick any number greater than 1, and then guess a number between 1 and what you chose.")
answerAccepted = False

while not answerAccepted:
    try:
        upperBound = int(input("What is your upper number: "))
        answerAccepted = True
    except ValueError:
        print("Invalid number.")

computerNumber = random.randint(1, upperBound)

playerNumber = 0

while playerNumber != computerNumber:
    isANum = True
    try:
        playerNumber = int(input("I'm thinking of a number between 1 and " + str(upperBound) + ". Guess what is is: "))
    except ValueError:
        print("That is not a number.")
        isANum = False
    if playerNumber > computerNumber and isANum:
        print("Your guess was too high!")
    if playerNumber < computerNumber and isANum:
        print("Your guess was too low!")

if playerNumber == computerNumber:
    print("Well guessed!")
