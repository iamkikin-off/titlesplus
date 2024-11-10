using GDWeave.Godot;
using GDWeave.Godot.Variants;
using GDWeave.Modding;

namespace TitlesPlus;

public class PlayerHud : IScriptMod {
    public bool ShouldRun(string path) => path == "res://Scenes/HUD/playerhud.gdc";

    public IEnumerable<Token> Modify(string path, IEnumerable<Token> tokens) {
        
        var waiter = new MultiTokenWaiter([
            t => t.Type == TokenType.PrFunction,
            t => t is IdentifierToken { Name: "_ready" },
            t => t.Type == TokenType.Colon,
            t => t.Type == TokenType.Newline,
        ], allowPartialMatch: true);
        
        var topOfFile = new MultiTokenWaiter([
            t => t.Type == TokenType.Newline,
            t => t.Type == TokenType.Newline,
        ], allowPartialMatch: true);

        
        // Inject PlayerHud
        foreach (var token in tokens) {
            
            if (topOfFile.Check(token)) {
                yield return token;

                // onready var multiTitle = get_node("/root/TitlesPlus")
                yield return new Token(TokenType.PrOnready);
                yield return new Token(TokenType.PrVar);
                yield return new IdentifierToken("multiTitle");
                
                yield return new Token(TokenType.OpAssign);
                
                yield return new IdentifierToken("get_node");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new ConstantToken(new StringVariant("/root/TitlesPlus"));
                yield return new Token(TokenType.ParenthesisClose);
                
                // New Line
                yield return new Token(TokenType.Newline);
                
                yield return token;
            } else if (waiter.Check(token)) {
                yield return token;
                
                yield return new IdentifierToken("multiTitle");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("_inject");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new Token(TokenType.Self);
                yield return new Token(TokenType.ParenthesisClose);
                
                // New Line
                yield return new Token(TokenType.Newline, 1);
            } else {
                yield return token;
            }
        }
    }
}
