import { Profile } from './profile';
export interface Activity {
    id: any;
    title: any;
    date: any;
    description: any;
    category: any;
    city: any;
    venue: any;
    hostUsername: any | null;
    isCancelled: any |null;
    isGoing: any | null;
    isHost: any | null;
    host?:Profile;
    attendees?:Profile[]
    
  }

  export class Activity implements Activity {
    constructor(init?:ActivityFormValues){
      Object.assign(this,init);
    }
  }

  export class ActivityFormValues {
    id?:string = undefined;
    title:string = '';
    category:string = '';
    description:string = '';
    date:Date|null = null;
    city:string = '';
    venue:string = '';

    constructor(activity?:ActivityFormValues){
      if(activity) {
        this.id = activity.id;
        this.title = activity.title;
        this.category = activity.category;
        this.description = activity.description;
        this.date = activity.date;
        this.venue = activity.venue;
        this.city = activity.city;
      }

    }
  
  }
  

 