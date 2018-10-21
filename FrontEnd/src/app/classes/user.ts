export class User {
    userName: string;
    name: string;
    surname: string;
    mail: string;
    password: string;
    role: string;

    constructor(userName: string, name: string, surname: string, password: string, mail: string, role: string) {
        this.userName = userName;
        this.name = name;
        this.surname = surname;
        this.password = password;
        this.mail = mail;
        this.role = role;
    }
}
