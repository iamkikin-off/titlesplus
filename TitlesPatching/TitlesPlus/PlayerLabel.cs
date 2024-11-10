using GDWeave.Godot;
using GDWeave.Godot.Variants;
using GDWeave.Modding;

namespace TitlesPatching;

public class PlayerLabel : IScriptMod {
    public bool ShouldRun(string path) => path == "res://Scenes/Entities/Player/player_label.gdc";

    public IEnumerable<Token> Modify(string path, IEnumerable<Token> tokens) {
        
        var waiter = new MultiTokenWaiter([
            t => t is IdentifierToken {Name: "_update_title"},
            t => t.Type is TokenType.Newline,
            t => t is IdentifierToken {Name: "label"},
            t => t.Type is TokenType.Newline,
        ], allowPartialMatch: true);
        
        var topOfFile = new MultiTokenWaiter([
            t => t.Type == TokenType.Newline,
            t => t.Type == TokenType.Newline,
        ], allowPartialMatch: true);
        
        // Connect a signal
        foreach (var token in tokens) {
            if (topOfFile.Check(token)) {
                yield return token;

                // onready var titles_plus = get_node("/root/TitlesPlus")
                yield return new Token(TokenType.PrOnready);
                yield return new Token(TokenType.PrVar);
                yield return new IdentifierToken("titles_plus");
                
                yield return new Token(TokenType.OpAssign);
                
                yield return new IdentifierToken("get_node");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new ConstantToken(new StringVariant("/root/TitlesPlus"));
                yield return new Token(TokenType.ParenthesisClose);
                
                // New Line
                yield return new Token(TokenType.Newline);
                
                // func _ready():
                yield return new Token(TokenType.PrFunction);
                yield return new IdentifierToken("_ready");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Colon);
                
                // New Line
                yield return new Token(TokenType.Newline, 1);
                
                // titles_plus.connect("_sendTitle", self, "_update_title")
                yield return new IdentifierToken("titles_plus");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("connect");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new ConstantToken(new StringVariant("_sendTitle"));
                yield return new Token(TokenType.Comma);
                yield return new Token(TokenType.Self);
                yield return new Token(TokenType.Comma);
                yield return new ConstantToken(new StringVariant("_update_title"));
                yield return new Token(TokenType.ParenthesisClose);

                // New Line
                yield return new Token(TokenType.Newline);
                
                yield return token;
            } else {
                yield return token;
            }
        }
    }
}
