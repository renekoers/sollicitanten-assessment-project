class Header {
    constructor() {
        this._puzzleTime = 150;
        this._puzzleNumber = 1;
        const puzzleInfoElement = document.getElementById("puzzle-info");
        const progressValueElement = puzzleInfoElement.querySelector(".progress-value");
        this._puzzleTimeMinutesElement = progressValueElement.querySelector("#puzzle-time-minutes");
        this._puzzleTimeSecondsElement = progressValueElement.querySelector("#puzzle-time-seconds");
        this._puzzleTimeProgressBarElement = puzzleInfoElement.querySelector(".progress-bar");
        this.setLevelNumber(puzzleInfoElement.querySelector("#puzzle-id"));
        this.startTimer();
    }
    static init() {
        new Header();
    }
    setLevelNumber(puzzleIdElement) {
        puzzleIdElement.innerText = this._puzzleNumber.toString();
    }
    startTimer() {
        this._puzzleTimeLeft = this._puzzleTime + 1;
        this.updateTimer();
        setInterval(() => this.updateTimer(), 1000);
    }
    updateTimer() {
        if (this._puzzleTimeLeft <= 0)
            return; // Halt this nonsense! We don't want to underflow into negative time!
        this._puzzleTimeLeft--;
        const time = new Time(this._puzzleTimeLeft);
        this.setTimerText(time);
        this.setProgressBarWidth();
    }
    setTimerText(time) {
        this._puzzleTimeMinutesElement.innerText = time.minutes.toString();
        // Ignore "padStart(number, string)" not found error. string.prototype.padStart(n,s) *does* in fact exist, so the error is invalid.
        // @ts-ignore 2339
        this._puzzleTimeSecondsElement.innerText = time.seconds.toString().padStart(2, "0");
    }
    setProgressBarWidth() {
        const timeLeftFraction = this._puzzleTimeLeft / this._puzzleTime;
        const timeLeftPercentage = timeLeftFraction * 100;
        this._puzzleTimeProgressBarElement.style.width = timeLeftPercentage.toString() + "%";
    }
}
class Time {
    constructor(seconds) {
        this._seconds = seconds;
    }
    get seconds() {
        return this._seconds % 60;
    }
    get minutes() {
        return Math.floor(this._seconds / 60);
    }
}
Header.init();
