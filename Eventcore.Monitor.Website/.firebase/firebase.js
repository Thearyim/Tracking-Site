import * as firebase from "firebase";

const config = {
apiKey: 'AIzaSyC6khmetKb7NoaOqdhqPw-1EYwulp0Fvng',
authDomain: "tracker-site-102e6.firebaseapp.com",
databaseURL: "https://tracker-site-102e6.firebaseio.com",
projectId: "tracker-site-102e6",
storageBucket: "tracker-site-102e6.appspot.com",
messagingSenderId: "894486821290"
};

export default !firebase.apps.length ? firebase.initializeApp(config) : firebase.app();