export class Log {
    userName: string;
    action: string;
    dateTime: Date;

    constructor(userName: string, action: string, dateTime: Date) {
        this.userName = userName;
        this.action = action;
        this.dateTime = dateTime;
    }
}
