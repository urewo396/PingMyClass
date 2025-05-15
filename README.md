# PingMyClass 🚨📚

A simple C# app that reads your weekly schedule from a CSV and sends you Telegram alerts 5 minutes before each lesson starts. ⏰✨

## Features 🚀

- Loads schedule from a CSV file (Monday to Friday lessons) 📅
- Sends Telegram messages as reminders 📲
- Keeps track to avoid duplicate alerts per day ✅
- Runs continuously and resets alerts at midnight 🌙

## How to use ⚙️

1. Fill in your schedule CSV file (see format below) 📝  
2. Put your Telegram bot token and chat ID in the `SendTelegramMessage` function 🔑  
3. Run the program — it handles the rest 🔥

## CSV format 🗂️
Day,Subject,StartTime
Monday,Math,08:00
Monday,PE,09:30
...

(you also have an example in this repo)


## Notes ⚠️

- Make sure your system time is correct 🕰️  
- Telegram bot needs permission to message you 🤖  
- The app runs in a loop — use with care 🔄  


