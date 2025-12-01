namespace api.Models;

public record Photo(
    string Url_165, // navbar 165 * 165 10KB
    string Url_256, // card / thumbnail 17KB
    string Url_enlarged, // enlarged photo up to ~300KB9oi 
    bool IsMain // profile => true ELSE => false
);

/* Anahita 5678209876345120309876

Folder name: 5678209876345120309876
    sub-folder 165 * 165, 
    sub-folder 256 * 256,
    sub-folder enlarged
*/