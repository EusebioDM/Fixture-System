export class Encounter {
    id: string;
    dateTime: string;
    teamIds: Array<string>;
    sportName: string;
    results: { };

    constructor(id: string, dateTime: string, teamIds: Array<string>, sportName: string, results: { }) {
        this.id = id;
        this.dateTime = dateTime;
        this.teamIds = teamIds;
        this.sportName = sportName;
        this.results = results;
    }
}
