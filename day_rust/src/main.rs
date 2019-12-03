fn main() {
    println!("Hello, world!");
}

#[test]
fn check_answer_validity() {
    assert_eq!(answer(), 42);
}

fn answer() -> u32 {
    42
}
