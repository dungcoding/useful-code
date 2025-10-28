const differenceInArrays = (array1, array2) =>  {
    const set = new Set(array2);
    return array1.filter(x => !set.has(x));
};