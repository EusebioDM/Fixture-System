export class User {
    userName: string;
    name: string;
    surname: string;
    mail: string;

    constructor(userName: string, name: string, surname: string, mail: string) {
        this.userName = userName;
        this.name = name;
        this.surname = surname;
        this.mail = mail;
    }
}
