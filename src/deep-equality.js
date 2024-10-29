function deepEqual(object1, object2) {
  const keys1 = Object.keys(object1);
  const keys2 = Object.keys(object2);
  if (keys1.length !== keys2.length) {
    return false;
  }
  for (const key of keys1) {
    const value1 = object1[key];
    const value2 = object2[key];
    const areObjects = isValidObject(value1) && isValidObject(value2);
    if (
      areObjects && !deepEqual(val1, val2) ||
      !areObjects && val1 !== val2
    ) {
      return false;
    }
  }
  return true;
}

function isValidObject(object) {
 return object !== null && typeof object === "object" ? true : false
}

export default deepEqual;