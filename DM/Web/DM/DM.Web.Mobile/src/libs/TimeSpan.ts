export default class TimeSpan {
    private readonly now: number;
    private sTime: Date;
    private readonly totalMilliseconds: number;
    private readonly totalSeconds: number;
    private readonly totalMinutes: number;
    private readonly totalHours: number;
    private totalDays: number;
    constructor (startTime: Date) {
        this.now = new Date().getTime()
        this.sTime = startTime
        this.totalMilliseconds = Math.abs(this.now - this.sTime.getTime())
        this.totalSeconds = this.totalMilliseconds / 1000
        this.totalMinutes = this.totalSeconds / 60
        this.totalHours = this.totalMinutes / 60
        this.totalDays = this.totalHours / 24
    }
    pastHumanize () {
        if (this.totalSeconds < 30) {
            return 'только что'
        }
        if (this.totalMinutes < 1) {
            return 'меньше минуты назад'
        }
        if (this.totalHours < 1) {
            let minutes = Math.floor(this.totalMinutes)
            let minutesFirstDivision = minutes % 10
            let pluralSuffix = minutes > 10 && minutes < 20 ||
            minutesFirstDivision > 4 || minutesFirstDivision === 0
                ? ''
                : (minutesFirstDivision === 1 ? 'у' : 'ы')
            return minutes === 1 ? 'минуту назад' : minutes + ' минут' + pluralSuffix + ' назад'
        }
        return ''
    }
}
