class Header
{
    private _puzzleTime : number = 150;
    private _puzzleTimeLeft : number;
    private _puzzleNumber : number = 1;
    private _puzzleTimeSecondsElement : HTMLElement;
    private _puzzleTimeMinutesElement : HTMLElement;
    private _puzzleTimeProgressBarElement : HTMLElement;

    public static init() : void
    {
        new Header();
    }

    private constructor()
    {
        const puzzleInfoElement = document.getElementById("puzzle-info");
        const progressValueElement = puzzleInfoElement.querySelector(".progress-value");

        this._puzzleTimeMinutesElement = progressValueElement.querySelector("#puzzle-time-minutes");
        this._puzzleTimeSecondsElement = progressValueElement.querySelector("#puzzle-time-seconds");
        this._puzzleTimeProgressBarElement = puzzleInfoElement.querySelector(".progress-bar");

        this.setLevelNumber(puzzleInfoElement.querySelector("#puzzle-id"));
        this.startTimer();
    }

    private setLevelNumber(puzzleIdElement : HTMLElement) : void
    {
        puzzleIdElement.innerText = this._puzzleNumber.toString();
    }

    private startTimer() : void
    {
        this._puzzleTimeLeft = this._puzzleTime + 1;
        this.updateTimer();
        setInterval(() => this.updateTimer(), 1000);
    }

    private updateTimer() : void
    {
        if(this._puzzleTimeLeft <= 0)
            return; // Halt this nonsense! We don't want to underflow into negative time!
        this._puzzleTimeLeft--;
        const time = new Time(this._puzzleTimeLeft);
        this.setTimerText(time);
        this.setProgressBarWidth();
    }
    
    private setTimerText(time : Time) : void
    {
        this._puzzleTimeMinutesElement.innerText = time.minutes.toString();
        // Ignore "padStart(number, string)" not found error. string.prototype.padStart(n,s) *does* in fact exist, so the error is invalid.
        // @ts-ignore 2339
        this._puzzleTimeSecondsElement.innerText = time.seconds.toString().padStart(2, "0");
    }

    private setProgressBarWidth() : void
    {
        const timeLeftFraction = this._puzzleTimeLeft / this._puzzleTime;
        const timeLeftPercentage = timeLeftFraction * 100;
        this._puzzleTimeProgressBarElement.style.width = timeLeftPercentage.toString() + "%";
    }
}

class Time
{
    private _seconds : number;

    public constructor(seconds : number)
    {
        this._seconds = seconds;
    }
    
    public get seconds() : number 
    {
        return this._seconds % 60;
    }
    
    public get minutes() : number
    {
        return Math.floor(this._seconds / 60);
    }
}

Header.init();