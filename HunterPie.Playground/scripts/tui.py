import sys
import msvcrt
from typing import List

def getch():
    ch = msvcrt.getch()
    # handle arrow keys
    if ch == b'\xe0' or ch == b'\x00':
        ch2 = msvcrt.getch()
        return {b'H': 'UP', b'P': 'DOWN', b'M': 'RIGHT', b'K': 'LEFT'}.get(ch2, None)
    return ch.decode()

# ANSI colors
class Color:
    CYAN = "\033[36m"
    YELLOW = "\033[33m"
    GREEN = "\033[32m"
    RESET = "\033[0m"

def clear_last_lines(n=1):
    """Clear the last n lines in terminal without wiping everything"""
    for _ in range(n):
        sys.stdout.write("\033[F\033[K")  # move cursor up + clear line
    sys.stdout.flush()

def ask_input(question: str) -> str:
    print(Color.CYAN + question + Color.RESET)
    ans = input("> ")
    clear_last_lines(2)
    return ans

def ask_choices(
        question: str, 
        options: List[str]) -> List[str]:
    print(Color.YELLOW + question + Color.RESET)
    idx = 0
    chosen = []

    options.append("Ok, I'm done!")

    while True:
        for i, opt in enumerate(options):
            marker = "[x]" if opt in chosen else "[ ]"
        
            if i == len(options) - 1:
                marker = ">" if i == idx else " "

            if i == idx:
                print(Color.GREEN + f"{marker} {opt}" + Color.RESET)
            else:
                print(f"{marker} {opt}")

        ch = getch()
        if ch == "UP" and idx > 0:
            idx -= 1
        elif ch == "DOWN" and idx < len(options) - 1:
            idx += 1
        elif ch in ("\n", "\r", None):
            if idx == len(options) - 1:
                clear_last_lines(len(options) + 1)
                return chosen

            choice = options[idx]
            
            if choice in chosen:
                chosen.remove(choice)
            else:
                chosen.append(choice)
        
        clear_last_lines(len(options))

# Example usage
if __name__ == "__main__":
    name = ask_text("What is your name?")
    color = ask_choices("Pick a color:", ["Red", "Green", "Blue"])
    print(f"Hello {name}, you picked {color}!")