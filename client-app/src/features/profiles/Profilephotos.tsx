import { observer } from "mobx-react-lite";
import { Interface } from "readline";
import { Button, Card, Grid, Header, Image, Tab } from "semantic-ui-react";
import { Photo, Profile } from "../../app/models/profile";
import { promises } from "dns";
import { useStore } from "../../app/stores/store";
import { SyntheticEvent, useState } from "react";
import PhotoUploadWidget from "../../app/common/imageApload/PhotoUploadWidget";
import { id } from "date-fns/locale";




interface Props {
    profile: Profile;
}
export default observer(function profileProfile({ profile }: Props) {
    const { profileStore: { isCurrentUser, uploadphoto, uploading, loading, setMainPhoto, deletePhoto } } = useStore();
    const [addPhotoMode, setPhotoMode] = useState(false);
    const [target, setTarget] = useState('')


    function handlePhotoUpload(file: Blob) {
        uploadphoto(file).then(() => setPhotoMode(false))
    }
    function HandleSetMainPhoto(photo: Photo, e: SyntheticEvent<HTMLButtonElement>) {
        setTarget(e.currentTarget.name);
        setMainPhoto(photo);
    }
    function handleDeletePhoto(photo: Photo, e: SyntheticEvent<HTMLButtonElement>) {
        setTarget(e.currentTarget.name);
        deletePhoto(photo)
    }
    return (

        <Tab.Pane>
            <Grid>
                <Grid.Column width={16}>
                    <Header floated="left" icon='image' content='photos' />
                    {isCurrentUser && (
                        <Button floated="right" basic content={addPhotoMode ? 'Cancel' : "Add Photo"}
                            onClick={() => setPhotoMode(!addPhotoMode)} />
                    )}
                </Grid.Column>
                <Grid.Column width={16}>
                    {addPhotoMode ? (
                        <PhotoUploadWidget uploadPhoto={handlePhotoUpload} loading={uploading} />
                    ) : (
                        <Card.Group itemsPerRow={5}>
                            {profile.photos?.map(photo => (
                                <Card key={photo.id} >
                                    <Image src={photo.url} />
                                    {isCurrentUser && (
                                        <Button.Group fluid widths={2}>
                                            <Button
                                                basic
                                                color="green"
                                                content='Main'
                                                name={'main' + photo.id}
                                                disabled={photo.ismain}
                                                loading={target === 'main' + photo.id && loading}
                                                onClick={(e: any) => HandleSetMainPhoto(photo, e)} />

                                            <Button basic color="red" icon='trash'
                                                loading={target === photo.id && loading}
                                                onClick={e => handleDeletePhoto(photo, e)}
                                                disabled={photo.ismain}
                                                name={photo.id}
                                            />
                                        </Button.Group>



                                    )}
                                </Card>
                            ))}
                        </Card.Group>
                    )}

                </Grid.Column>
            </Grid>


        </Tab.Pane>
    )
})