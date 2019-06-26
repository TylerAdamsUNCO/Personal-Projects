#This is my first time ever coding something in Ruby so go easy on me.

puts("Let's play a guessing game.")
puts("You're going to pick any number greater than 1, and then guess a number between 1 and what you chose.")
upperBound = 0

until upperBound > 1
    puts("What is your upper number: ")
    upperBound = gets.to_i
    if upperBound <= 1
    puts("Invalid number.")
    end
end

computerNumber = Random.rand(1...upperBound)
playerNumber = 0

while playerNumber != computerNumber
    isANum = true
    puts("I'm thinking of a number between 1 and #{upperBound}. Guess what it is: ")
    playerNumber = gets.to_i
    if playerNumber < 1
        puts("Invalid number.")
        isANum = false
    end
    if playerNumber > computerNumber and isANum
        puts("Your guess was too high!")
    end
    if playerNumber < computerNumber and isANum
        puts("Your guess was too low!")
    end
end

if playerNumber == computerNumber
    puts("Well guessed!")
end
