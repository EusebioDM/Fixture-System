export class Encounter {
    id: string;
    dateTime: string;
    TeamsIds: Array<string>;
    sportName: string;
    results: { };

    constructor(id: string, dateTime: string, TeamsIds: Array<string>, sportName: string, results: { }) {
        this.id = id;
        this.dateTime = dateTime;
        this.TeamsIds = TeamsIds;
        this.sportName = sportName;
        this.results = results;
    }
}
