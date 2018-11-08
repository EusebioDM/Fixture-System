export class Encounter {
    id: string;
    dateTime: string;
    teamsId: Array<string>;
    sportName: string;

    constructor(id: string, dateTime: string, teamsId: Array<string>, sportName: string) {
        this.id = id;
        this.dateTime = dateTime;
        this.teamsId = teamsId;
        this.sportName = sportName;
    }
}
