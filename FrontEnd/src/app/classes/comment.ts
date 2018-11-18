export class Comment {
    id: string;
    userName: string;
    timeStamp: string;
    message: string;
    encounterId: string;

    constructor(id: string, userName: string, timeStamp: string, message: string, encounterId: string) {
        this.id = id;
        this.userName = userName;
        this.timeStamp = timeStamp;
        this.message = message;
        this.encounterId = encounterId;
    }
}
