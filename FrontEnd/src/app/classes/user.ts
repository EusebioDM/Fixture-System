export class User {
    userName: string;
    name: string;
    surname: string;
    password: string;
    mail: string;
    isAdmin: boolean;

    constructor(userName: string, name: string, surname: string, password: string, mail: string, isAdmin: boolean) {
        this.userName = userName;
        this.name = name;
        this.surname = surname;
        this.password = password;
        this.mail = mail;
        this.isAdmin = isAdmin;
    }
}
