export class Encounter {
    id: string;
    dateTime: string;
    TeamsIds: Array<string>;
    sportName: string;

    constructor(id: string, dateTime: string, TeamsIds: Array<string>, sportName: string) {
        this.id = id;
        this.dateTime = dateTime;
        this.TeamsIds = TeamsIds;
        this.sportName = sportName;
    }
}
