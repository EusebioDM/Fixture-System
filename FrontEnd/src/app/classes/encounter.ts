export class Encounter {
    id: string;
    dateTime: string;
    teamsIds: Array<string>;
    sportName: string;

    constructor(id: string, dateTime: string, teamsIds: Array<string>, sportName: string) {
        this.id = id;
        this.dateTime = dateTime;
        this.teamsIds = teamsIds;
        this.sportName = sportName;
    }
}
