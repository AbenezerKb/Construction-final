export default function ValidDate(dateObject){

    if (new Date(dateObject).toString() !== 'Invalid Date'){
        dateObject = new Date(
            dateObject
        ).toLocaleString();
    }
    else{
        dateObject = null;
    }
    return dateObject
}