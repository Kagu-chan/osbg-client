class Person {
  constructor(name: string, age: number) {
    this.name = name;
    this.age = age;
  }
  name: string;
  age: number;
}

function whoAmI(person: Person) {
  console.log("I am " + person.name + " and im " + person.age.toString() + " years old.");
}

let p = new Person("Kai", 23);
whoAmI(p);