import { observer } from "mobx-react-lite";
import { Image, List, Popup } from "semantic-ui-react";
import { Profile } from "../../../app/models/profile";
import { Link } from "react-router-dom";
import ProfileCard from "../../profiles/ProfileCard";

interface Props{
    attendees:Profile[];
}

export default observer(function ActivityListItemAttendee({attendees}:Props){

    const styles = {
        borderColor : 'orange',
        borderWidth:2
    }

    return(
        <List horizontal>

            {attendees.map(attendees =>(
                <Popup hoverable
                key={attendees.username}
                trigger={
                    <List.Item key={attendees.username} as={Link} to={`/profiles/${attendees.username}`}>
                    <Image 
                    size='mini' 
                    circular src ={attendees.image ||'/assets/user.png'}
                    bordered
                    style ={attendees.following ? styles : null}
                    
                    />
                </List.Item>

                }>
                    <Popup.Content>
                        <ProfileCard profile={attendees}></ProfileCard>
                    </Popup.Content>

                </Popup>
            ))}
            
            
        </List>
        
    )
})