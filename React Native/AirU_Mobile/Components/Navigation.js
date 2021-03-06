import {createBottomTabNavigator, createStackNavigator} from 'react-navigation'
import {Platform} from 'react-native'
import Icon from 'react-native-vector-icons/Ionicons'
import React from 'react';
import {Image} from 'react-native'

// home
import Home from '../Views/Home'
import Settings from '../Views/Settings/Settings'
import Map from '../Views/Map'
import Setup from '../Views/Setup/SetupNew'
import MountingSensor from '../Views/Setup/MountingSensor'

// setup
import Login from '../Views/Setup/Login'
import ReviewFirst from '../Views/Setup/ReviewFirst'
import Privacy from '../Views/Setup/Privacy'
import ConnectionSetup from '../Views/Setup/ConnectionSetup'
import Confirmation from '../Views/Setup/Confirmation'

import PollutionInfo from '../Views/Tracker/PollutionInfo'

// sensor
import Sensor from '../Views/Sensor/Sensor'

// sensor if needed
import RegisterAccount from '../Views/Setup/RegisterAccount'

// tracker
import TrackerMenu from '../Views/Tracker/TrackerMenu'
import Tracker from '../Views/Tracker/Tracker'

// settings if needed
import EditDevice from '../Views/Settings/EditDevice'
import Refresh from '../Views/Settings/Refresh'
import Notifications from '../Views/Settings/Notifications'

/**
 * Navigator used for moving between screens using react-navigation library. 
 * Update any new screen routes here as needed. Note: there should only be 1 
 * navigator in a project, so do not add another stackNavigator
 */

// Tabs across bottom of screens
const TabNavigator = createBottomTabNavigator(
  {
    Sensor: {screen: Sensor},
    Track: {screen: TrackerMenu},
    Map: {screen: Map},
    Settings: {screen: Settings}
  },
  {
    navigationOptions: ({navigation}) => ({
      tabBarIcon: ({ focused, horizontal, tintColor }) => {
        const {routeName} = navigation.state
        
        // Icons for bottom page, if update above add image here as well
        if (routeName == 'Sensor') {
          return <Icon name={Platform.OS === "ios" ? "ios-analytics" : "md-analytics"} size={20}/>
        }
        else if (routeName == 'Track') {
          return <Icon name={Platform.OS === "ios" ? "ios-bonfire" : "md-bonfire"} size={20}/>
        }
        else if (routeName == 'Map') {
          return <Icon name={Platform.OS === "ios" ? "ios-map" : "md-map"} size={20}/>
        }
        else if (routeName == 'Settings') {
          return <Icon name={Platform.OS === "ios" ? "ios-settings" : "md-settings"} size={20}/>
        }
        else
          return <Image/>
        }
      })
  }
);

// top level navigator
const Router = createStackNavigator(
  {
    // home or tabs
    Home: {screen: Home},
    Settings: {screen: Settings}, 
    Map: {screen: Map},
    Track: {screen: TrackerMenu},
    Tabs: {screen: TabNavigator, initalRouteName: 'Home'},

    // sensor setup
    Setup: {screen: Setup},
    Login: {screen: Login},
    RegisterAccount: {screen: RegisterAccount}, 
    ReviewFirst: {screen: ReviewFirst}, 
    Privacy: {screen: Privacy}, 
    Confirmation: {screen: Confirmation}, 
    ConnectionSetup: {screen: ConnectionSetup},
    MountingSensor: {screen: MountingSensor},
    
    //pollution info linked to from tracker page
    PollutionInfo: {screen: PollutionInfo},

    //sensor screens
    Sensor: {screen: Sensor},

    //tracker screens
    Tracker: {screen: Tracker},

    // Settings
    EditDevice: {screen: EditDevice},
    Refresh: {screen: Refresh},
    Notifications: {screen: Notifications}
  },
  {
    initalRouteName: 'Home',
    navigationOptions: ({navigation}) => ({
      header: null
    }),
  },
);

export default Router