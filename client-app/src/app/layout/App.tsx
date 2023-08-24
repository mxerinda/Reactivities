import React, { useEffect, useState } from 'react';
import './styles.css';
import NavBar from './NavBar';
import axios from 'axios';
import { Header, List ,Container, Loader, Button} from 'semantic-ui-react';
import {Activity} from '../models/activity';
import ActivityDashboard from '../../features/activities/dashboard/ActivityDashboard'
import {v4 as uuid} from 'uuid';
import agent from '../api/agent';
import LoadingComponent from '../../app/layout/LoadingComponent';
import { useStore } from '../stores/store';
import { observer } from 'mobx-react-lite';
import { Route, Switch, useLocation } from 'react-router-dom';
import HomePage from '../../features/home/HomePage'
import ActivityForm from '../../features/activities/form/ActivityForm';
import ActivityDetails from '../../features/activities/details/ActivityDetails';
import TestErrors from '../../features/errors/TestError';
import ServerError from '../../features/errors/ServerError';
import { ToastContainer } from 'react-toastify';
import NotFound from '../../features/errors/NotFound';
import LoginForm from '../../app/users/LoginForm';
import ModalContainer from'../../app/common/modals/ModalContainer';
import ProfilePage from'../../features/profiles/ProfilePage';

function App() {

const location = useLocation();
const {commonStore,userStore} = useStore(); 

useEffect(() => {
  if(commonStore.token){
    userStore.getUser().finally(() => commonStore.setAppLoaded())
  } else {
    commonStore.setAppLoaded();
  }
},[commonStore,userStore])

if(!commonStore.appLoaded) return <LoadingComponent content='Loading app...' />




  // const {activityStore} = useStore();
  // const [activities, setActivities] = useState<Activity[]>([]);
  // const [selectedActivity, setSelectedActivity] = useState<Activity | undefined>(undefined);
  // const [editMode,setEditMode] = useState(false);
  // const [loading,setLoading] = useState(true);
  // const [submitting, setSubmitting] = useState(false);

  // useEffect(() => {
  //  activityStore.loadActivities();
  // }, [activityStore])

  // if(activityStore.loadingInitial) return <LoadingComponent content='Loading app' />

  // function handleSelectedActivity(id:string){
  //   setSelectedActivity(activities.find(x => x.id === id))
  // }

  // function handleCancelSelectActivity(){
  //   setSelectedActivity(undefined);
  // }

  // function handleFormOpen(id?: string){
  //   id ? handleSelectedActivity(id) : handleCancelSelectActivity();
  //   setEditMode(true);
  // }

  // function handleFormClose(){
  //   setEditMode(false);
  // }

  // function handleCreateOrEditActivity(activity:Activity){
  //   setSubmitting(true);
  //   if(activity.id){
  //     agent.Activities.update(activity).then(()=> {
  //       setActivities([...activities.filter(x => x.id !== activity.id),activity]);
  //       setSelectedActivity(activity);
  //       setEditMode(false);
  //       setSubmitting(false);
  //     });
  //   }
  //   else{
  //     activity.id = uuid();
  //     agent.Activities.create(activity).then(()=>{
  //       setActivities([...activities,activity]);
  //       setSelectedActivity(activity);
  //       setEditMode(false);
  //       setSubmitting(false);
  //     })
  //   }
  // }

  // function handleDeleteActivity(id:string){
  //   setSubmitting(true);
  //   agent.Activities.delete(id).then(()=>{
  //     setActivities([...activities.filter(x => x.id !== id)]);
  //     setSubmitting(false);
  //   })
  // }



  return (
    <>
    <ToastContainer position='bottom-right' hideProgressBar />
    <ModalContainer />
    <Route exact path='/' component={HomePage} />
    <Route
      path={'/(.+)'}
      render={()=> (
        <>
          <NavBar/>
    <Container style={{marginTop:'7em'}}>
      <Switch>
      <Route exact path ='/' component={HomePage} />
    <Route exact path = '/activities' component={ActivityDashboard} />
    <Route path = '/activities/:id' component={ActivityDetails} />
    <Route key={location.key} path = {['/createActivity','/manage/:id']} component={ActivityForm} />
    <Route path='/profiles/:username' component={ProfilePage} />
    <Route path='/errors' component={TestErrors} />
    <Route path='/server-error' component={ServerError} />
    <Route path='/server-error' component={ServerError} />
    <Route path='/login' component={LoginForm} />
    <Route component={NotFound} />
      </Switch>
    </Container>


        </>
      )}
      />
   
    </>
  );
}

export default observer(App);
